import * as React from 'react'

type FieldProps = {
    name: string,
    label: string,
    type: string,
    autoComplete: string,
    required: boolean,
}

const Field: React.FunctionComponent<FieldProps> = ({ name, label, type, autoComplete, required }) => {
    return (
        <div>
            <label id={[name, 'label'].join('-')} htmlFor={[name, 'input'].join('-')}>
                {label} {required ? <span title="Required">*</span> : undefined}
            </label>
            <br />
            <input
                autoComplete={autoComplete}
                id={[name, 'input'].join('-')}
                name={name}
                required={required}
                type={type}
            />
        </div>
    )
}

export default Field