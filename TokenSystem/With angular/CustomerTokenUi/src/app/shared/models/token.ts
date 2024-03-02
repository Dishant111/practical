export interface TokenListModel
{
    id:number,
    query:number,
    status:number
}

export interface Token
{
    id:number,
    queryId:number,
    statusId:number,
    phone:string,
    address :string
}


export interface ResolveToken
{
    id:number,
    query:number,
    phoneNumber:string,
    address :string
}


export interface TokenCreateModel
{
    query:number
}