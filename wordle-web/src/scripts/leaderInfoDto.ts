import type { CivAttribute } from "./civAttribute";
import type { LeaderAttribute } from "./leaderAttribute";

export class LeaderInfoDto {
    leaderName: string = ''
    civName: string = ''
    civAttributes: CivAttribute[] = []
    leaderAttributes: LeaderAttribute[] = []
  }