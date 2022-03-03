export interface Pagination { 
currentPage: number ; 
itemsPerPage: number ;
totalItems: number; 
totalPages: number;

}

//generic class T can represent "anything"
export class PaginatedResult<T> {
    result: T;
    pagination: Pagination;
} 