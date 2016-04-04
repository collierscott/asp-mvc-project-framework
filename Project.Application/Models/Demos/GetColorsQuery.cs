using System;
using Infrastructure.Data;

namespace Project.Application.Models.Demos
{
    public class GetColorsQuery : SqlQuery
    {

        public GetColorsQuery(string facilities)
        {

            Id = "GetColorsQuery";

            Query = string.Format(@"
                SELECT
                (
                    SELECT html_bg_color 
                    FROM Projectapp.ctr_p_chart_items
                    WHERE item_name = 'TARGET'
                        AND facility_all_or_one = '{0}'
                ) AS target_color,
                (
                    SELECT html_bg_color
                    FROM Projectapp.ctr_p_chart_items
                    WHERE item_name = 'PLANNED'
                        AND facility_all_or_one = '{0}'
                ) AS commit_color
                FROM dual",
                !string.IsNullOrEmpty(facilities) && !facilities.Equals("ALL", StringComparison.OrdinalIgnoreCase) ? "ONE" : "ALL"
            );


        }
         
    }
}