import type { Attribute } from "./attribute";

export class LeaderInfoDto {
    leaderName: string = ''
    civName: string = ''
    civAttributes: Attribute[] = []
    leaderAttributes: Attribute[] = []
  }