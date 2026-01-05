import { ResApi } from "./base.interface";
import { UserList } from "./userList.interface";

export interface UserListResponse
    extends ResApi<UserList[]> {}