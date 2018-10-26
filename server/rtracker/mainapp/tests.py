from django.test import TestCase
from . import models
# Create your tests here.


class FlowTest(TestCase):

    def setUp(self):
        flow = models.ReportFlow.objects.create(
            specification='od-base.pd.ard.rd')
        self.fielduser = models.ROUser(Office='pso-borong')
        self.fielduser.Flow = flow
        self.fielduser.save()

        self.pduser = models.ROUser.objects.create(Office='pd')
        self.od_baseuser = models.ROUser.objects.create(Office='od-base')

    def blank_flow_is_false(self):
        self.assertTrue(not self.pduser.Flow)

    def correct_flow_behaviour(self):
        flow = models.ReportFlow(specification='od-base.pd.ard.rd')
        self.assertEquals(flow.next_of('od-base', 'pd'))
        self.assertEquals(flow.next_of('rd', 'rd'))
        with self.assertRaises(models.ReportEndOfFlowException):
            flow.previous_of('od-base')

    def test_flow(self):
        report = models.Report(Owner=self.fielduser)
        report.save()

        report.start_tracking(self.fielduser)
        self.assertEqual(report.CurrentOffice, 'od-base')

        with self.assertRaises(models.UserNoPermissionToChangeReportState):
            report.approve(self.fielduser)

        self.assertEqual(report.CurrentOffice, 'od-base')

        report.approve(self.od_baseuser)
        self.assertEqual(report.CurrentOffice, 'pd')

        report.reject(self.pduser)
        self.assertEqual(report.CurrentOffice, 'pso-borong')
