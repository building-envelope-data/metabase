import * as React from 'react'

export type HeaderProps = {
    appTitle: string
}

export const Header: React.FunctionComponent<HeaderProps> = ({ appTitle }) => (
    <span>
        { appTitle}
    </span>
);

export default Header;