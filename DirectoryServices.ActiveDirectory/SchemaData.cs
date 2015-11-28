/*=====================================================================
  File:     SchemaData.cs

  Summary:  Demonstrates how to retrieve information about 
            an AD schema. The first method displays information about
            the entire schema. The second method displays information about
            a single class in the schema. The third method displays information
            about a single property (attribute) in the schema.

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
    public class SchemaData
    {
        
        public static void GetSchemaData()
        {

            Console.WriteLine();
            Console.WriteLine("<--SCHEMA Information-->\n");

            ActiveDirectorySchema schema;

            try
            {
                // bind to the schema in the current forest
                schema = ActiveDirectorySchema.GetCurrentSchema();
            }
            catch (ActiveDirectoryObjectNotFoundException e)
            {
                // can't bind to the schema
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("Current schema: {0}\n", schema);

            // get all the abstract classes in the schema
            Console.WriteLine("All abstract schema classes:");
            foreach (ActiveDirectorySchemaClass schemaClass in
                                schema.FindAllClasses(SchemaClassType.Abstract))
            {
                Console.WriteLine(schemaClass);
            }

            // get all the defunct classes in the schema
            Console.WriteLine("\nAll defunct schema classes:");
            foreach (ActiveDirectorySchemaClass schemaClass in
                                                 schema.FindAllDefunctClasses())
            {
                Console.WriteLine(schemaClass);
            }

            // get all attributes that are indexed 
            // and replicated to global catalog
            Console.WriteLine("\nAll indexed attributes that are also " +
                                               "replicated to the global catalog:");
            foreach (ActiveDirectorySchemaProperty schemaProperty in
                                schema.FindAllProperties(
                                                 PropertyTypes.Indexed |
                                                 PropertyTypes.InGlobalCatalog))
            {
                Console.WriteLine(schemaProperty);
            }

        }

        
        public static void GetSchemaClassData(string className)
        {

            Console.WriteLine("<--SCHEMA CLASS INFORMATION-->\n");

            // get a forest context
            DirectoryContext context = new DirectoryContext(
                                            DirectoryContextType.Forest);

            ActiveDirectorySchemaClass schemaClass;

            try
            {
                // bind to a class schema object
                schemaClass = ActiveDirectorySchemaClass.FindByName(context,
                                                                    className);
            }
            catch (ArgumentException e)
            {
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
            Console.WriteLine("\nMandatoryProperties:");
            
            foreach (ActiveDirectorySchemaProperty schemaProperty in
                                                schemaClass.MandatoryProperties)
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
            Console.WriteLine("SchemaGuid: {0}", schemaClass.SchemaGuid);

        }

        public static void GetSchemaPropertyData(string propertyName)
        {

            Console.WriteLine("<--SCHEMA ATTRIBUTE INFORMATION-->\n");

            // get a forest context
            DirectoryContext context = new DirectoryContext(
                                            DirectoryContextType.Forest);

            ActiveDirectorySchemaProperty schemaProperty;

            try
            {
                // bind to an attribute schema object
                schemaProperty = ActiveDirectorySchemaProperty.FindByName(
                                                                  context,
                                                                  propertyName);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                Console.WriteLine("Schema property \"{0}\" could not be found", propertyName);
                return;
            }

            Console.WriteLine("Name: {0}", schemaProperty.Name);
            Console.WriteLine("Oid: {0}", schemaProperty.Oid);
            Console.WriteLine("Description: {0}", schemaProperty.Description);

            // limits on the length of the property (in this case this is length
            // these could also mean upper and lower limits 
            // values for the attribute)
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

            // other characteristics of the property
            Console.WriteLine("IsIndexed: {0}", schemaProperty.IsIndexed);
            Console.WriteLine("IsInGlobalCatalog: {0}",
                                              schemaProperty.IsInGlobalCatalog);

        }

    }
}
