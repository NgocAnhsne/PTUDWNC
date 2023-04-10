import axios from 'axios';
import { get_api } from './Methods'; 

export async function getPosts (keyword = '',
    pageSize = 10,
    pageNumber = 1,
    sortColumn = '',
    sortOrder = '') {
        return
    get_api(`https://localhost:7085/api/posts?keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
} 
export function getAuthors(name = '',
 pageSize = 10,
 pageNumber = 1,
 sortColumn = '',
 sortOrder = '') {
 return
    get_api(`https://localhost:7085/api/authors?name=${name}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}
export function getFilter() {
    return get_api('https://localhost:7085/api/posts/get-filter');
}
