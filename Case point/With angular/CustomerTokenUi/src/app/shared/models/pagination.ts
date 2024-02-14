export interface Pagination{
     totalCount:number,
     pagesize:number,
     pageNumber:number,
}

export interface PaginationResult<T> extends Pagination{
     data :T[] 
}