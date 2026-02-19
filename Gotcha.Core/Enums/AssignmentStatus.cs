namespace Gotcha.Core.Enums
{
    public enum AssignmentStatus
    {
        Ongoing, // use ongoing when the target & hunter are still alive and the assignment is active
        Killed, // use killed when the target was successfully killed
        Failed,
        Cancelled, // use cancelled when the hunter died
        Revoked // use revoked when the admin removes the assignment
    }
}
