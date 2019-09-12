from django.shortcuts import render
from django.http import HttpRequest, HttpResponse

from icon.models import User, Product, MeasurementMethod
from icon.serializers import UserSerializer, ProductSerializer, MeasurementMethodSerializer

from rest_framework import generics

def home(request: HttpRequest) -> HttpResponse:
    return render(request, 'icon/home.html', {"title": "Icon Knowledge Database"})

class UserListCreate(generics.ListCreateAPIView):
    queryset = User.objects.all()
    serializer_class = UserSerializer

class ProductListCreate(generics.ListCreateAPIView):
    queryset = Product.objects.all()
    serializer_class = ProductSerializer

class MeasurementMethodListCreate(generics.ListCreateAPIView):
    queryset = MeasurementMethod.objects.all()
    serializer_class = MeasurementMethodSerializer
