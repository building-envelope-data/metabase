from django.db import models
from django.urls import reverse
from django.core.validators import MinLengthValidator
from django.contrib.postgres.fields import JSONField

# Organizing models in a package: https://docs.djangoproject.com/en/2.2/topics/db/models/#organizing-models-in-a-package

# def randomString(string_length=16):
#     letters = string.ascii_lowercase
#     return ''.join(random.choice(letters) for i in range(string_length))

Identifier = models.CharField(unique=True, null=False, max_length=16, validators=[MinLengthValidator(16)])

class Identifiable(models.Model):
    identifier = Identifier
    name = models.TextField(null=False)
    description = models.TextField(null=False)

    class Meta:
        abstract = True
        ordering = ['name']
        indexes = [models.Index(fields=['identifier'])]

class User(Identifiable):
    def url(self):
        return reverse('user', args=[self.identifier])

class Product(Identifiable):
    owner = models.ForeignKey(User, on_delete=models.CASCADE)
    # private_information = JSONField(null=False)

    def url(self):
        return reverse('product', args=[self.identifier])

class MeasurementMethod(Identifiable):
    owner = models.ForeignKey(User, on_delete=models.CASCADE)

    def url(self):
        return reverse('measurement-method', args=[self.identifier])
