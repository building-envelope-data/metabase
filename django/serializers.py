from rest_framework import serializers

from icon.models import User, Product, MeasurementMethod

class IdentifiableSerializer(serializers.ModelSerializer):
    class Meta:
        fields = ("identifier", "name", "description")

class UserSerializer(IdentifiableSerializer):
    class Meta(IdentifiableSerializer.Meta):
        model = User

class ProductSerializer(IdentifiableSerializer):
    class Meta(IdentifiableSerializer.Meta):
        model = Product

class MeasurementMethodSerializer(IdentifiableSerializer):
    class Meta(IdentifiableSerializer.Meta):
        model = MeasurementMethod
