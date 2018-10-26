from rest_framework import serializers
from . import models


class ReportSerializer(serializers.ModelSerializer):
    class Meta:
        model = models.Report
        fields = '__all__'


class ROUserSerializer(serializers.ModelSerializer):
    class Meta:
        model = models.ROUser
        fields = '__all__'
        depth = 1