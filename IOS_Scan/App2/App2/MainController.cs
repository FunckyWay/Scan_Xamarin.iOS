using UIKit;

namespace App2
{
    public  class MainController :UITabBarController
    {
        

        public MainController()
        {
            var sc = new Scan();
            sc.NavigationItem.Title = "test";
            UINavigationController unc1 = new UINavigationController(sc);
            unc1.TabBarItem = new UITabBarItem(UITabBarSystemItem.Favorites, 0);

            var cbc = new CreateBarCode();
            UINavigationController unc2 = new UINavigationController(cbc);
            unc2.TabBarItem = new UITabBarItem(UITabBarSystemItem.Contacts, 1);

            var cqc = new CreateQC();
            UINavigationController unc3 = new UINavigationController(cqc);
            unc3.TabBarItem = new UITabBarItem(UITabBarSystemItem.Bookmarks, 2);

          
            var tabs = new UIViewController[]
            {
              unc1,unc2,unc3
            };

            ViewControllers = tabs;
            
        }

        //public override void ItemSelected(UITabBar tabbar, UITabBarItem item)
        //{
        //    if(item.Tag == 0)
        //    {
        //        this.NavigationController.PushViewController(new Scan(),true);
        //    }
        //}



    }
}