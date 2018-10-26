from django.shortcuts import render
from rest_framework import views, viewsets, generics
from . import models, serializers
from django.db.models import Q
from rest_framework import response, serializers as drf_serializers, status
from django.http import JsonResponse
# Create your views here.


class ReportListView(generics.ListCreateAPIView):

    serializer_class = serializers.ReportSerializer

    def get_queryset(self):
        filtered = models.Report.objects.exclude(
            CurrentState=models.STATE_UNTRACKED)
        return filtered.filter(Q(LastOffice=self.request.user.rouser.Office) | Q(CurrentOffice=self.request.user.rouser.Office))

    def perform_create(self, serializer):
        document = serializer.save(Owner=self.request.user.rouser)
        document.start_tracking()


class AuthCheckView(views.APIView):

    def get(self, request):
        rouser = request.user.rouser
        serializer = serializers.ROUserSerializer(rouser)
        return response.Response(data=serializer.data, status=status.HTTP_200_OK)
