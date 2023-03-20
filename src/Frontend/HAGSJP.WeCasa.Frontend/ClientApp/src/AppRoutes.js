import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { Registration } from "./components/Registration";
import { Login } from "./components/Login";
import { GroupSettings } from "./components/GroupSettings";
import { ProfileSettings } from "./components/ProfileSettings";
import { AccountSettings } from "./components/AccountSettings";
import { Files } from "./components/Files";
import { FileView } from "./components/FileView";
import { IconSelectorModal } from "./components/IconSelectorModal";


const AppRoutes = [
  {
    index: true,
    element: <Registration />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
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
  }
];

export default AppRoutes;
