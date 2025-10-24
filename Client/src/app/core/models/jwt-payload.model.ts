import { JwtPayload } from 'jwt-decode'; 
export interface MyJwtPayload extends JwtPayload 
{ 
    displayname?: string;
    email?: string; 
    issuperuser?: string; 
    permissions?: string; 
    roleassign?: string; 
    scope?: string; 
    username?: string; 
    avatar?: string; 
}