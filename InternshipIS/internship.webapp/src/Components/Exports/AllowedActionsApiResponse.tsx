import {AllowedActions} from './AllowedActions';

export interface AllowedActionsApiResponse {
    statusCode: number;
    message: string | null;
    data: {
        allowedActions: AllowedActions;
        statusCode: number;
        message: string | null;
    };
    errors: string[];
    hasErrors: boolean;
}