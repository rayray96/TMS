import { PersonModel } from '.';

export interface TeamModel {
    teamName: string;
    team: PersonModel[];
}
