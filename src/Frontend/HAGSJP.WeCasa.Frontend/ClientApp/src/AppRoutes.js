import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Registration } from "./components/Registration";
import { Login } from "./components/Login";
import { BudgetBar } from "./components/BudgetBar/BudgetBar";

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
    path: '/finances',
    element: <BudgetBar />
   }
  // {
  //   path: '/',
  //   element: <Home />
  // },
  // {
  //   path: '/home',
  //   element: <Home />
  // }
];

export default AppRoutes;
