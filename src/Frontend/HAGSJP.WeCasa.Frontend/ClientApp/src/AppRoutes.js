import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { Registration } from "./components/Registration";
import { Login } from "./components/Login";
import { BudgetBar } from "./components/BudgetBar/BudgetBar";
import { GroupSettings } from "./components/GroupSettings";


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
    path: '/finances',
    element: <BudgetBar />
    }
];

export default AppRoutes;