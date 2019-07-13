namespace Employees.Data
{
    public class F0601161Row : Celin.AIS.Row
    {
        public int F0601161_ANPA { get; set; }
        public string F0601161_DST { get; set; }
        public string F0601161_HMCU { get; set; }
        public string F0601161_DT { get; set; }
        public string F0601161_JBCD { get; set; }
        public string F0601161_JBST { get; set; }
        public string F0601161_ALPH { get; set; }
        public int F0601161_AN8 { get; set; }
        public string F0601161_HMLC { get; set; }
        public bool Connected { get; set; }
    }

    public class F0601161Response : Celin.AIS.FormResponse
    {
        public Celin.AIS.Form<Celin.AIS.FormData<F0601161Row>> fs_DATABROWSE_F0601161;
    }

    public class F0601161DatabrowserRequest : Celin.AIS.DatabrowserRequest
    {
        public F0601161DatabrowserRequest()
        {
            dataServiceType = "BROWSE";
            targetName = "F0601161";
            targetType = "table";
            returnControlIDs = "ALPH|AN8|ANPA|DST|DT|HMCU|HMLC|JBCD|JBST";
            maxPageSize = "5000";
            outputType = "GRID_DATA";
            query = new Celin.AIS.Query
            {
                matchType = "MATCH_ALL",
                condition = new[]
                {
                    new Celin.AIS.Condition
                    {
                        value = new[]
                    {
                            new Celin.AIS.Value
                            {
                                content = "0",
                                specialValueId = "LITERAL"
                            }
                        },
                        controlId = "F0601161.DT",
                        @operator = "EQUAL"
                    }
                }
            };
        }
    }
}
