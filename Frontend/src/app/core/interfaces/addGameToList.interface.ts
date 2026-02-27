import { GameInfo } from './gameInfo.interface';

export interface AddGameToList {
  userListId: number;
  gameInfo: GameInfo;
  statusId?: number;
}
