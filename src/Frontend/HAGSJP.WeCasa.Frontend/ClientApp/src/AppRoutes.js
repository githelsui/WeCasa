import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { Registration } from "./components/Registration";
import { Login } from "./components/Login";
import { GroupSettings } from "./components/GroupSettings";
import { AccountSettings } from "./components/AccountSettings";
import { Files } from "./components/Files";
import { FileView } from "./components/FileView";


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
  }
];

export default AppRoutes;
