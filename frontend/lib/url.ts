export function isLocalUrl(url: string): boolean
{
    return new URL(document.baseURI).origin === new URL(url, document.baseURI).origin;
}
