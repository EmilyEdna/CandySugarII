using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class MenuModel
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public static List<MenuModel> GetMenus()
        {
            return new List<MenuModel>
            {
                new MenuModel  { Name="新番",Icon="ico_0.png"},
                new MenuModel  { Name="动漫",Icon="ico_1.png"},
                new MenuModel  { Name="壁纸",Icon="ico_2.png"},
                new MenuModel  { Name="漫画",Icon="ico_3.png"},
                new MenuModel  { Name="小说",Icon="ico_4.png"},
                new MenuModel  { Name="文学",Icon="ico_5.png"},
                new MenuModel  { Name="电影",Icon="ico_6.png"},
                new MenuModel  { Name="音乐",Icon="ico_7.png"},
               /* new MenuModel  { Name="太阳",Icon="ico_8.png"},
                new MenuModel  { Name="月亮",Icon="ico_9.png"},
                new MenuModel  { Name="运动",Icon="ico_0_1.png"},*/
            };
        }
    }
}
