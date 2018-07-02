using masts_dvr_tool.DataAccess.Contracts;
using masts_dvr_tool.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masts_dvr_tool.DataAccess.Repository
{
    public class MastRepository : IMastRepository
    {
        private string connectionString;

        public MastRepository(string connectionString)
        {
            //Workaround to address Connection String issue
            if (connectionString.Contains("\\\\"))
                connectionString = connectionString.Replace("\\\\", "\\");

            this.connectionString = connectionString;
        }

        public Mast GetMastData(string sharing, string uid, string mastData = "0")
        {
            Mast mastDataObject = new Mast();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetTemplateData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Masts", SqlDbType.Int).Value = mastData;
                    command.Parameters.AddWithValue("@Sharing", SqlDbType.Int).Value = sharing;
                    command.Parameters.AddWithValue("@UID", SqlDbType.VarChar).Value = uid;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    int rowCount = 0;
                    while (reader.Read())
                    {
                        if (rowCount == 0) //Data is duplicated in numerous rows
                        {
                            mastDataObject.Address1 = reader["Address1"].ToString();
                            mastDataObject.VOAAddressLine2 = reader["Address2"].ToString();
                            mastDataObject.VOAAddressLine3 = reader["Address3"].ToString();
                            mastDataObject.VOAAddressLine4 = reader["Address4"].ToString();
                            mastDataObject.VOAEffectiveFrom = reader["EffectiveDate"].ToString();
                            mastDataObject.VOABAName = reader["BA Name"].ToString();
                            mastDataObject.VOAShared = Convert.ToDouble(reader["NumberOfSharers"]) > 0 ? "Yes" : "No";
                            mastDataObject.VOASiteType = reader["Location type"].ToString();
                            mastDataObject.VOAMastStructureType = "Not sure what database field";

                        }

                        rowCount++;
                    }

                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM [NBS2017_Masts].[dbo].[CMV2017D_Valuation] where UID = @UID";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@UID", SqlDbType.VarChar);
                    command.Parameters["@UID"].Value = uid;

                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        mastDataObject.VOABARefNumber = reader["BA Ref"].ToString();
                        mastDataObject.VOAMastOperator = reader["Operator"].ToString();
                        mastDataObject.VOASiteRef = reader["SiteRef"].ToString();
                        mastDataObject.VOACtil = reader["CTIL"].ToString();
                        mastDataObject.VOAEastings = reader["Eastings"].ToString();
                        mastDataObject.VOANorthings = reader["Northings"].ToString();
                        mastDataObject.VOAMastHeight = reader["Height"].ToString();
                        mastDataObject.VOASiteArea = reader["SiteArea"].ToString();
                        mastDataObject.VOAM25 = reader["InsideM25"].ToString() == "NULL" ? "No" : "Yes";
                        mastDataObject.VOACellType = reader["MastType"].ToString();
                        mastDataObject.VOARateableValue = $"£{reader["CalculatedRV"]}";
                    }


                }

            }
            return mastDataObject;
        }
    }
}
