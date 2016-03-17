/*=====================================================================
  File:     ForestConfigData.cs

  Summary:  Section for retrieving information about the current
            forest using the Forest class and the 
            GetCurrentForest method   
---------------------------------------------------------------------
                        **                         **
*/

using System;
using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;


//[assembly: System.Reflection.AssemblyVersion("1.0.0.0")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]

namespace MSDN.Samples.DirectoryServices.ActiveDirectory
{
    public partial class FromForest
    {

        public static void GetConfigData()
        {

            Console.WriteLine("<--FOREST INFO-->\n");

            Forest forest;

            try
            {
                // bind to the current forest
                forest = Forest.GetCurrentForest();
            }
            catch (ActiveDirectoryObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("Current forest: {0}", forest.Name);
            Console.WriteLine();

            // list all domains in this forest
            Console.WriteLine("\nDomains in the forest:");
            foreach (Domain domain in forest.Domains)
            {
                Console.WriteLine("\t{0}", domain.Name);
            }

            // list all global catalogs within the forest
            Console.WriteLine("\nGlobal catalogs in the forest:");
            foreach (GlobalCatalog gc in forest.GlobalCatalogs)
            {
                Console.WriteLine("\t{0}", gc.Name);
            }

            // list FSMO and schema role owner in the forest
            Console.WriteLine("\nRole owners:");
            Console.WriteLine("\tNamingRole: {0}", forest.NamingRoleOwner);
            Console.WriteLine("\tSchemaRole: {0}", forest.SchemaRoleOwner);


            Console.WriteLine("\nApplication partitions in the forest:");
            
            //using the Forest class to obtain partition information
            ApplicationPartitionCollection appPartitions =
                                            forest.ApplicationPartitions;

            foreach (ApplicationPartition appPartition in appPartitions)
            {
                Console.WriteLine("\t{0}", appPartition.Name);
                
            }

        }
    }
}
