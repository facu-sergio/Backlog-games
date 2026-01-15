import { ResApi } from "./base.interface";
import { GameInfo } from "./gameInfo.interface";
import { UserList } from "./userList.interface";

export interface UserListResponse
    extends ResApi<UserList[]> {}

export interface GamesByListResponse
    extends ResApi<GameInfo[]>{}