from django.db import models
from django.contrib.auth.models import User, Group
from django.core import exceptions
# Create your models here.

STATE_UNTRACKED = 'untracked'
STATE_REJECTED = 'rejected'


class ReportEndOfFlowException(Exception):
    pass


class UserNoPermissionToChangeReportState(Exception):
    pass


class Report(models.Model):
    ReportTitle = models.CharField(max_length=256)
    Owner = models.ForeignKey('ROUser', on_delete=models.SET_NULL, null=True)
    CurrentOffice = models.CharField(
        max_length=32, null=True, blank=True, default='')
    CurrentState = models.CharField(
        max_length=32, null=True, blank=True, default='')
    LastOffice = models.CharField(
        max_length=32, null=True, blank=True, default='')

    NeedsReview = models.BooleanField(default=False)
    WasSent = models.BooleanField(default=False)

    def __check_user_can_process_report(self, rouser):
        retval = self.CurrentOffice == rouser.Office and \
            not self.CurrentState in [STATE_UNTRACKED, STATE_REJECTED]
        if retval:
            return retval

        raise UserNoPermissionToChangeReportState()

    def __update_last_office(self, rouser):
        self.NeedsReview = False
        self.LastOffice = rouser.Office

    def start_tracking(self, rouser):

        if not rouser.Flow:
            raise exceptions.ValidationError(
                message='User must set flow to use tracking.')

        self.CurrentOffice = self.Owner.Flow.first()
        self.__update_last_office(rouser)
        self.save()

    def approve(self, rouser):
        #import pdb; pdb.set_trace()
        self.__check_user_can_process_report(rouser)
        next_office = self.Owner.Flow.next_office(self.CurrentOffice)
        self.CurrentOffice = next_office
        self.__update_last_office(rouser)
        self.NeedsReview = False
        self.save()
        

    def return_back(self, rouser):
        if not self.__check_user_can_process_report(rouser):
            return

        try:
            next_office = self.Owner.Flow.previous_of(self.CurrentOffice)
            self.CurrentOffice = next_office
            self.save()
        except ReportEndOfFlowException:
            self.CurrentOffice = rouser.Office

        self.__update_last_office(rouser)
        self.save()

    def reject(self, rouser):
        if not self.__check_user_can_process_report(rouser):
            return

        self.CurrentState = STATE_REJECTED
        self.CurrentOffice = self.Owner.Office
        self.__update_last_office(rouser)
        self.save()

    def for_review(self, rouser):
        if not self.__check_user_can_process_report(rouser):
            return

        self.NeedsReview = True
        self.__update_last_office(rouser)
        self.save()

    def sent(self, rouser):
        self.WasSent = True
        self.NeedsReview = False
        self.save()

    def untrack(self, rouser):
        self.Untracked = True
        self.NeedsReview = False
        self.save()


class ROUser(models.Model):
    _User = models.OneToOneField(User, on_delete=models.SET_NULL, null=True)
    Flow = models.ForeignKey(
        'ReportFlow', on_delete=models.SET_NULL, null=True)
    Office = models.CharField(max_length=16)


class ReportFlow(models.Model):

    specification = models.CharField(max_length=64, null=False, blank=False)

    def __get_offices(self):
        return self.specification.split('.')

    def next_office(self, office):
        offices = self.__get_offices()
        if office in offices:
            idx = offices.index(office)+1
            if idx >= len(offices):
                return offices[-1]
            return offices[idx]
        raise exceptions.ValidationError(
            message='Target Office not found in report flow.')

    def first(self):
        offices = self.__get_offices()
        return offices[0]

    def previous_office(self, office):
        offices = self.__get_offices()
        if office in offices:
            idx = offices.index(office)-1
            if idx < 0:
                raise ReportEndOfFlowException()
            return offices[idx]
        raise exceptions.ValidationError(
            message='Target Office not found in report flow.')
