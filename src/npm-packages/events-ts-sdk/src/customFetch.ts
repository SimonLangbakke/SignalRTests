// Modified version of: https://github.com/orval-labs/orval/blob/master/samples/next-app-with-fetch/custom-fetch.ts

// NOTE: Supports cases where `content-type` is other than `json`
const getBody = <T>(c: Response | Request): Promise<T> => {
    const contentType = c.headers.get('content-type');

    if (contentType && contentType.includes('application/json')) {
        return c.json();
    }

    if (contentType && contentType.includes('application/pdf')) {
        return c.blob() as Promise<T>;
    }

    return c.text() as Promise<T>;
};

// NOTE: Update just base url
const getUrl = (contextUrl: string): string => {
    const baseUrl = process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL;
    // console.log(contextUrl);
    if (contextUrl.startsWith("/")) {
        return `${baseUrl}${contextUrl}`;
    }

    const url = new URL(contextUrl);
    const pathname = url.pathname;
    const search = url.search;

    // TODO: add custom base url logic here
    // const baseUrl = process.env.NEXT_PUBLIC_EVENTS_API_BASE_URL;
    // process.env.NODE_ENV === 'production'
    //   ? 'productionBaseUrl'
    //   : 'http://localhost:3000';

    const requestUrl = new URL(`${baseUrl}${pathname}${search}`);

    return requestUrl.toString();
};

// NOTE: Add headers
const getHeaders = (headers?: HeadersInit): HeadersInit => {
    return {
        ...headers,
        Authorization: 'token',
        'Content-Type': 'multipart/form-data',
    };
};

export const customFetch = async <T>(
    url: string,
    options: RequestInit,
): Promise<T> => {
    const requestUrl = getUrl(url);
    //const requestHeaders = getHeaders(options.headers);

    const requestInit: RequestInit = {
        ...options
    };

    // console.log(requestUrl);
    const res = await fetch(requestUrl, requestInit);

    const body = [204, 205, 304].includes(res.status) ? null : await res.text();

    const data = body ? JSON.parse(body) : {};
    return { data, status: res.status, headers: res.headers } as T;
};