import { Home } from "./components/Home";
import { Registration } from "./components/Registration/Registration";
import { Login } from "./components/Login/Login";
import { BudgetBar } from "./components/BudgetBar/BudgetBar";
import { CalendarView } from "./components/Calendar/CalendarView";
import { GroupSettings } from "./components/Group/GroupSettings";
import { ProfileSettings } from "./components/Profile/ProfileSettings";
import { AccountSettings } from "./components/Account/AccountSettings";
import { AccountRecovery } from "./components/Account/AccountRecovery";
import { Files } from "./components/File/Files";
import { FileView } from "./components/File/FileView";
import { IconSelectorModal } from "./components/IconSelectorModal";
import BulletinBoard from "./components/BulletinBoard/BulletinBoard";
import { ChoreList } from "./components/ChoreList/ChoreList";
import { GroceryList } from "./components/GroceryList/GroceryList";
import { AnalyticsDashboard } from "./components/Dashboard/AnalyticsDashboard";
import { NotFound } from "./components/NotFound";

const AppRoutes = [
  {
    index: true,
    element: <Registration />
  },
  {
      path: '/registration',
      element: <Registration />
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/',
    element: <Home />
  },
  {
    path: '/home',
    element: <Home />
  },
  {
      path: '/group-settings',
      element: <GroupSettings />
  },
  {
    path: '/finances',
    element: <BudgetBar />
    },
    {
        path: '/calendar',
        element: <CalendarView />
    },
    {
        path: '/account-settings',
        element: <AccountSettings />
    },
    {
        path: '/account-recovery',
        element: <AccountRecovery />
    },
    {
     path: '/edit-profile',
     element: <ProfileSettings />
  },
  {
    path: '/files',
    element: <Files />
  },
  {
    path: '/file-view-development',
    element: <FileView />
  },
  {
     path: '/icon-selector-dev',
     element: <IconSelectorModal />
  },
  {
        path: '/chorelist',
        element: <ChoreList />
  },
  {
      path: '/grocerylist',
      element: <GroceryList />
  },
  {
  path: '/bulletin',
  element: <BulletinBoard />
  },
  {
    path: '/analytics',
    element: <AnalyticsDashboard />
  },
  {
        path: '*',
        element: <NotFound />
  }
];

export default AppRoutes;