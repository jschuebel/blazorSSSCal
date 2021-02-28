using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSSCalBlazor.Client.Models
{
    public class PeopleModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string homePhone { get; set; }
        public string pager { get; set; }
        public string work { get; set; }
        public string eMail { get; set; }
        public int AddressId { get; set; }
        public bool birthdayAlert { get; set; }
        //createdate":"1999-11-10T00:00:00","address":null,"events
    }


    public class EventModel
    {
        public int id { get; set; }
        public string description { get; set; }
        public string displayDescription
        {
            get
            {
                var len = 0;
                var newstring = "";
                if (description == null || (description != null && description.Trim().Length == 0))
                {
                    newstring = this.topic == null ? "N/A" : userName + " " + this.topic.Trim();
                    if (newstring.Trim().Length > 25)
                        newstring = newstring.Substring(0, 25).Trim();
                    len = newstring.Length;
                }
                else
                {
                    if (this.description.Contains("@fmt"))
                    {
                        return this.description.Substring(4);
                    }
                    if (description.Trim().Length > 25)
                        newstring = description.Substring(0, 25).Trim();
                    else
                        newstring = description.Trim();
                    len = newstring.Length;
                }
                len = (len > 25) ? 25 : len;
                var padd = new string[27 - len];
                var padds = string.Join("&nbsp;", padd);

                if (id != -1)
                    return newstring + padds + (date == null ? "N/A" : date.Value.ToString("MM-dd-yyyy"));
                else
                    return newstring;

            }
        }
        public string userName { get; set; }
        public int userId { get; set; }
        public int topicId { get; set; }
        public DateTime? date { get; set; }
        public bool? repeatYearly { get; set; }
        public bool? repeatMonthly { get; set; }
        public string topic { get; set; }
        public Topic topicf { get; set; }

        public string topicTitle { get { return topicf?.topicTitle; } set { topicf.topicTitle=value; } }
        

    }

    public class Topic
    {
        //{"id":1,"topicTitle":"Birthday","createdate":"1999-11-10T00:00:00"}
        public int id { get; set; }
        public string topicTitle { get; set; }
    }

    public class GroupModel
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public int personId { get; set; }
        PeopleModel[] people { get; set; }
    }
    public class PictureNode
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FileExtension { get; set; }
        public string FileType { get; set; }
        public string CellType { get; set; }
        public Dictionary<string, PictureNode> folders { get; set; }

    }
}
