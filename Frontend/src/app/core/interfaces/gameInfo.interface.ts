import { GameStatus } from './game-status.type';

export interface GameInfo {
  id?: number;
  igdbId: number;
  userListId?: number;
  name: string;
  coverUrl?: string;
  firstReleaseDate?: number;
  summary?: string;
  rating?: number;
  gameStatusId?: number;
  gameStatusName?: GameStatus;
}