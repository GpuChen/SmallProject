using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using KMAPI.Models; // 物件引用
using Dapper;
using System.Data.SqlClient;

namespace KMAPI.Controllers
{
    [RoutePrefix("api/KM")]
    public class KMController : ApiController // System.Web.Http.ApiController
    {
        string _con = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SAEE_KM"].ConnectionString;
        SqlConnection sqlcon;
        Doc[] Docs = new Doc[] { };
        Tag[] Tags = new Tag[] { };
        [HttpGet]
        [Route("Doc")]
        public IEnumerable<Doc> getDoc(string title)
        {
            using (sqlcon = new SqlConnection(_con))
            {
                // 可以加入萬用自符 %%
                sqlcon.Open();
                string _sql = "SELECT * FROM SAI_KM WHERE 1=1 and title LIKE '" + title + "' ";
                Docs = sqlcon.Query<Doc>(_sql).ToArray();
            }
            return Docs;
        }

        [HttpGet]
        [Route("Doc")]
        public IEnumerable<Doc> getDoc(string category, string subCategory)
        {
            // 可以加入萬用自符 %%

            using (sqlcon = new SqlConnection(_con))
            {
                List<string> search = new List<string>();
                search.Add((category == null) ? "" : " and category LIKE '" + category + "' ");
                search.Add((subCategory == null) ? "" : " and subCategory LIKE '" + subCategory + "' ");

                sqlcon.Open();
                string _sql = "SELECT * FROM SAI_KM WHERE 1=1";
                foreach (string s in search)
                {
                    _sql += s;
                }
                Docs = sqlcon.Query<Doc>(_sql).ToArray();
            }
            return Docs;
        }
        [HttpGet]
        [Route("Tag")]
        public IEnumerable<Tag> getTag(string category, string subCategory, string enabled)
        {

            using (sqlcon = new SqlConnection(_con))
            {
                List<string> search = new List<string>();
                search.Add((category == null) ? "" : " and category LIKE '" + category + "' ");
                search.Add((subCategory == null) ? "" : " and subCategory LIKE '" + subCategory + "' ");
                search.Add((enabled.Equals("Y")) ? " and km.enabled = 'Y' " : "");

                sqlcon.Open();
                string _sql =
                        "SELECT DISTINCT tag ";
                _sql += "FROM SAI_KM_TAG tag ";
                _sql += "WHERE 1=1 AND tag.id IN ";
                _sql += "(SELECT km.id FROM SAI_KM_TAG tag LEFT JOIN SAI_KM KM ON tag.id = km.id WHERE 1=1 ";
                foreach (string s in search)
                {
                    _sql += s;
                }
                _sql += ")";

                Tags = sqlcon.Query<Tag>(_sql).ToArray();
            }
            return Tags;
        }


    }

}
