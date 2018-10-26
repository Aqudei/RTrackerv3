from rest_framework import permissions

class IsROUserPermission(permissions.BasePermission):


    def has_permission(self, request, view):
        try:
            return request.user.rouser
        except Exception:
            return False