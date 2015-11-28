/*=====================================================================
  File:     DomainControllerInfoDomainContext.cs

  Summary:  Demonstrates finding and returning information about a dc 
            in an Active Directory domain by using variations of the
            FineOne method.
---------------------------------------------------------------------
                        ** SAMPLE CODE **
*/

using System;
using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]


namespace MSDN.Samples.DirectoryServices.ActiveDirectory
{
    public partial class FromDomain
    {

        public static void GetDomainControllerConfigData()
        {

            Console.WriteLine("<--DOMAIN CONTROLLER CONFIGURATION DATA-->\n");

            DirectoryContext domainContext = new DirectoryContext(
                                                    DirectoryContextType.Domain);

            DomainController dc;

            try
            {
                // bind to a domain using the provided domain context
                dc = DomainController.FindOne(domainContext);
                Console.WriteLine("Domain controller in the current domain: {0}", dc);
                Console.WriteLine("Site: {0}", dc.SiteName);
                Console.WriteLine("Is global catalog: {0}", dc.IsGlobalCatalog());
                Console.WriteLine("Current Time: {0}", dc.CurrentTime.ToLocalTime());
                Console.WriteLine("IP Address: {0}\n", dc.IPAddress);
            }
            catch (ActiveDirectoryObjectNotFoundException e)
            {
                // dc not found
                Console.WriteLine(e.Message);
            }

            // finding any DC in a specific site
            // site name used to find a DC in a specific site
            // string siteName = "<replaceWithaSiteName>";
            string siteName = "Default-First-Site-Name";

            try
            {
                // bind to any dc in a specific site using the current domain context
                dc = DomainController.FindOne(domainContext, siteName);
                Console.WriteLine("Domain controller in the current" +
                                         " domain and site \"{0}\":", siteName);
                Console.WriteLine(dc);
                Console.WriteLine("Site: {0}", dc.SiteName);
                Console.WriteLine("Is global catalog: {0}\n", dc.IsGlobalCatalog());
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                // dc not found
                Console.WriteLine("Domain controller in site {0} not found.",
                                                                      siteName);
            }

            // finding KDC, but not if on the local computer 
            // (this example assumes that the code is running from a dc).
            try
            {
                // bind to a domain controller using the current domain context
                // and locator options
                dc = DomainController.FindOne(domainContext,
                                                LocatorOptions.KdcRequired |
                                                LocatorOptions.AvoidSelf);
                Console.WriteLine("KDC in the current domain:");
                Console.WriteLine(dc);
                Console.WriteLine("Site: {0}", dc.SiteName);
                Console.WriteLine("Is global catalog: {0}", dc.IsGlobalCatalog());
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                // kdc not found or no other dc answered
                Console.WriteLine(
                    "KDC not found in current domain or no other dc responded.");
            }

        }
    }
}