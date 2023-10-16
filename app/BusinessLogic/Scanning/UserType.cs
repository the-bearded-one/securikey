using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Scanning
{
    public class UserType : IChecker
    {
        public bool IsElevatedUser{ get; private set; }

        public void Scan()
        {
            EventAggregator.Instance.FireEvent(BlEvents.CheckingElevatedUser);

            // Get the Windows identity of the user running the application
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            // Create a Windows principal object based on that identity
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            // Check if the user is in the Administrator role
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (isAdmin)
            {
                IsElevatedUser = true;
            }
            else
            {
                IsElevatedUser = false;
            }

            EventAggregator.Instance.FireEvent(BlEvents.CheckingElevatedUserCompleted);
        }

    }
}
