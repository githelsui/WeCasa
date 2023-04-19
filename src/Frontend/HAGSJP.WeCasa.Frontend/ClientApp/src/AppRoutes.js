import { Home } from "./components/Home";
import { Registration } from "./components/Registration/Registration";
import { Login } from "./components/Login/Login";
import { BudgetBar } from "./components/BudgetBar/BudgetBar";
import { GroupSettings } from "./components/Group/GroupSettings";
import { ProfileSettings } from "./components/Profile/ProfileSettings";
import { AccountSettings } from "./components/Account/AccountSettings";
import { Files } from "./components/File/Files";
import { FileView } from "./components/File/FileView";
import { IconSelectorModal } from "./components/IconSelectorModal";
import { ChoreList } from "./components/ChoreList/ChoreList";


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
        path: '/account-settings',
        element: <AccountSettings />
    },
    {
     path: '/edit-profile',
     element: <ProfileSettings />
  },
  {
    path: '/account-settings',
    element: <AccountSettings />
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
    }
];

export default AppRoutes;