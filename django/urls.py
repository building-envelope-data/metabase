"""icon URL Configuration

The `urlpatterns` list routes URLs to views. For more information please see:
    https://docs.djangoproject.com/en/2.2/topics/http/urls/
Examples:
Function views
    1. Add an import:  from my_app import views
    2. Add a URL to urlpatterns:  path('', views.home, name='home')
Class-based views
    1. Add an import:  from other_app.views import Home
    2. Add a URL to urlpatterns:  path('', Home.as_view(), name='home')
Including another URLconf
    1. Import the include() function: from django.urls import include, path
    2. Add a URL to urlpatterns:  path('blog/', include('blog.urls'))
"""
from django.contrib import admin
from django.urls import path, re_path

from rest_framework_simplejwt.views import (
    TokenObtainPairView,
    TokenRefreshView,
    TokenVerifyView,
)

from icon import views

urlpatterns = [
    path('', views.home, name='home'),
    path('api/user/', views.UserListCreate.as_view()),
    re_path(r'^api/token/$', TokenObtainPairView.as_view(), name='obtain_token_pair'),
    re_path(r'^api/token/refresh/$', TokenRefreshView.as_view(), name='refresh_token'),
    re_path(r'^api/token/verify/$', TokenVerifyView.as_view(), name='verify_token'),
    # re_path(r'^user/(?P<identifier>[a-z]{16})$', views.user, name='user'),
    # path('products', views.products, name='products'),
    # re_path(r'^product/(?P<identifier>[a-z]{16})$', views.product, name='product'),
    # path('measurement-methods', views.measurement_methods, name='products'),
    # re_path(r'^measurement-method/(?P<identifier>[a-z]{16})$', views.measurement_method, name='measurement_method'),
    # path('admin/', admin.site.urls),
]
