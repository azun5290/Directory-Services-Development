/*=====================================================================
  File:     ADAMData.cs 

  Summary:  Demonstrates how to retrieve information about 
  the currrent schema. The first method display information about
  the entire schema. The second method displays information about
  a single class in the schema. The third method display information
  about a single property (attribute) in the schema.
  
  Note: This sample uses localhost for simplicity. With localhost, 
        you must run this sample on the machine running this ADAM 
        instance on the appropriate port number. Otherwise, you can 
        specify a FQDN or NetBIOS name to another machine running
        the ADAM instance. A common port number for ADAM on non-domain 
        controllers is 389 for a non-ssl connection. 50000 is the default
        for a non-ssl connection to an ADAM instance running on a 
        domain controller. 
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
    public class AdamData
    {

        // connect to an ADAM instance using s.ds.ad
        static string adamConnectionString = "localhost:50000";

        static DirectoryContext adamContext = new
            DirectoryContext(DirectoryContextType.DirectoryServer, adamConnectionString);


        public static void GetAdamInstanceData()
        {

            AdamInstance adamInstance;

            try
            {
                adamInstance = AdamInstance.GetAdamInstance(adamContext);
            }
            // This catch block runs if the ADAM connection string is 
            // invalid or the server can't be reached
            catch (ActiveDirectoryObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("<--ADAM Instance Information-->\n");

            Console.WriteLine("Configuration set {0}", adamInstance.ConfigurationSet);

            // get the roles of this ADAM instance
            AdamRoleCollection roles = adamInstance.Roles;

            Console.WriteLine("\nADAM Roles\n");
            foreach (AdamRole role in roles)
            {
                Console.WriteLine("\t{0}", role.ToString());
            }

            Console.WriteLine("\nADAM Partitions\n");

            // get the partitions of this ADAM instance
            ReadOnlyStringCollection partitions =
                                    adamInstance.Partitions;

            try
            {
                foreach (string partition in partitions)
                {
                    Console.WriteLine("\t{0}", partition.ToString());
                }
            }
            catch (ActiveDirectoryObjectNotFoundException e)
            {
                // This exception occurs if the partitions container 
                // can't be found. 
                Console.WriteLine(e.Message);
            }
        }

        public static void GetSchemaData()
        {

            Console.WriteLine("<--SCHEMA Information-->\n");

            ActiveDirectorySchema schema;

            try
            {
                // bind to the schema associated with the ADAM instance
                schema = ActiveDirectorySchema.GetSchema(adamContext);

            }
            catch (ActiveDirectoryObjectNotFoundException e)
            {
                // current context could not be obtained
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("Current schema: {0}", schema);

            // get all class names
            Console.WriteLine("\nAll schema classes:");
            foreach (ActiveDirectorySchemaClass schemaClass in
                                schema.FindAllClasses())
            {
                Console.WriteLine("Common name: {0}\n\tlDAPDisplayName: {1}",
                    schemaClass.CommonName, schemaClass.Name);
            }


            // get all the abstract classes in the schema
            Console.WriteLine("\nAll abstract schema classes:");
            foreach (ActiveDirectorySchemaClass schemaClass in
                                schema.FindAllClasses(SchemaClassType.Abstract))
            {
                Console.WriteLine(schemaClass);
            }

            // get all the defunct classes in the schema
            // This searches for all classes with the isDefunct attribute set to True
            // You cannot instantiate a defunct class. Since you cannot delete an 
            // attribute, setting an attribute to defunct is the next best thing.
            // By default, an ADAM instance doesn't contain any defunct classes.
            Console.WriteLine("\nAll defunct schema classes:");
            foreach (ActiveDirectorySchemaClass schemaClass in
                                   schema.FindAllDefunctClasses())
            {
                Console.WriteLine(schemaClass);
            }

            Console.WriteLine("\nAll defunct schema attributes:");

            foreach (ActiveDirectorySchemaProperty schemaProperty in
                                schema.FindAllDefunctProperties())
            {
                Console.WriteLine(schemaProperty);
            }

            Console.WriteLine("\nIndexed attributes:");

            foreach (ActiveDirectorySchemaProperty schemaProperty in
                                schema.FindAllProperties(
                                                 PropertyTypes.Indexed))
            {
                Console.WriteLine(schemaProperty);
            }

        }

        public static void GetSchemaClassData(string className)
        {

            Console.WriteLine("<--ADAM SCHEMA CLASS-->\n");

            ActiveDirectorySchemaClass schemaClass;

            try
            {
                // bind to an ADAM schema class object
                schemaClass = ActiveDirectorySchemaClass.FindByName(adamContext,
                                                                    className);
            }
            catch (ArgumentException e)
            {
                // this exception could be thrown if the current context 
                // is not associated to an instance of ADAM
                Console.WriteLine(e.Message);
                return;
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                Console.WriteLine("Schema class \"{0}\" could not be found", className);
                return;
            }

            Console.WriteLine("Name: {0}", schemaClass.Name);
            Console.WriteLine("Oid: {0}", schemaClass.Oid);
            Console.WriteLine("Description: {0}", schemaClass.Description);
            Console.WriteLine("SchemaGuid: {0}", schemaClass.SchemaGuid);
            Console.WriteLine("\nMandatoryProperties:");

            foreach (ActiveDirectorySchemaProperty schemaProperty in
                                                schemaClass.MandatoryProperties)
            {
                Console.WriteLine(schemaProperty);
            }

            Console.WriteLine("\nOptionalProperties:");
            foreach (ActiveDirectorySchemaProperty schemaProperty in
                                                schemaClass.OptionalProperties)
            {
                Console.WriteLine(schemaProperty);
            }

            Console.WriteLine("\nPossible Superiors:");
            foreach (ActiveDirectorySchemaClass supClass in
                                                  schemaClass.PossibleSuperiors)
            {
                Console.WriteLine(supClass);
            }
            Console.WriteLine("\nSubClassOf: {0}", schemaClass.SubClassOf);

        }


        public static void GetSchemaPropertyData(string propertyName)
        {
            Console.WriteLine("<--ADAM SCHEMA PROPERTY-->\n");

            ActiveDirectorySchemaProperty schemaProperty;

            try
            {
                // bind to an ADAM schema attribute object
                schemaProperty = ActiveDirectorySchemaProperty.FindByName(
                                                                  adamContext,
                                                                  propertyName);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                Console.WriteLine("Schema property \"{0}\" could not be found",
                                    propertyName);
                return;
            }

            Console.WriteLine("Name: {0}", schemaProperty.Name);
            Console.WriteLine("Oid: {0}", schemaProperty.Oid);
            Console.WriteLine("Description: {0}", schemaProperty.Description);
            Console.WriteLine("Schema GUID: {0}", schemaProperty.SchemaGuid);
            Console.WriteLine("IsIndexed: {0}", schemaProperty.IsIndexed);
            Console.WriteLine("IsSingleValued: {0}",
                                              schemaProperty.IsSingleValued);

            Console.WriteLine("\nUpper and lower value/length:");
            try
            {
                Console.WriteLine("RangeLower: {0}", schemaProperty.RangeLower);
                Console.WriteLine("RangeUpper: {0}", schemaProperty.RangeUpper);
            }
            catch (ArgumentNullException)
            {
                // if these properties are not defined for the schema property, 
                // ArgumentNullException is thrown
                Console.WriteLine("RangeLower/RangeUpper not defined.");
            }

        }

    }
}
