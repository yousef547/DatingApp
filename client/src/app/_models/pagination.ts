export interface Pagination {
    currentPage:number;
    itemsPage: number;
    totalItems:number;
    totalPages:number;
}

export class PaginationResult<T>{
    result: T;
    pagination:Pagination;
}