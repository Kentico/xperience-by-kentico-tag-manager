import { type FormComponentProps } from '@kentico/xperience-admin-base';
import Parse from 'html-react-parser';
import React from 'react';
import Select, { type ActionMeta, type OptionProps, type SingleValue, type SingleValueProps, type StylesConfig, components} from 'react-select';

export interface TagManagerSnippetDto {
    displayName: string;
    typeName: string;
    icon: string;
}

export interface TagManagerSnippetTypeDropdownComponentClientProperties
    extends FormComponentProps {
    value: string;
    snippetTypes: TagManagerSnippetDto[];
}

interface OptionType {
    value: string;
    label: string;
    icon: string;
}

const badgeStyle: React.CSSProperties = {
    height: '30px',
    display: 'flex',
    alignItems: 'center',
    gap: '10px',
    color: 'purple'
};

const singleValueStyle: React.CSSProperties =
{
    height: '30px',
    display: 'flex',
    alignItems: 'center',
    gap: '10px',
};

export const TagManagerSnippetTypeDropdownFormComponent = (
    props: TagManagerSnippetTypeDropdownComponentClientProperties
): JSX.Element => {

    const options: OptionType[] = props.snippetTypes.map(snippet => ({
        value: snippet.typeName,
        label: snippet.displayName,
        icon: snippet.icon,
    }));

    const safeParse = (html: string): React.ReactNode => {
        return (Parse as (input: string) => React.ReactNode)(html);
    };

    const CustomSingleValue = ({ data }: SingleValueProps<OptionType>): JSX.Element => (
        <div style={singleValueStyle} className="css-1dimb5e-singleValue">
            {data.icon ? safeParse(data.icon) : ''}
            {data.label}
        </div>
    );

    const CustomOption = (props: OptionProps<OptionType, false>): JSX.Element => (
        <components.Option {...props}>
            <div style={badgeStyle} className="css-1dimb5e-singleValue">
                {props.data.icon ? safeParse(props.data.icon) : ''}
                {props.data.label}
            </div>
        </components.Option>
    );

    const Save = (newValue: SingleValue<OptionType>, action: ActionMeta<OptionType>): void => {
        if (action.action === 'select-option') {
            const tagManagerSnippet: TagManagerSnippetDto =
            {
                icon: newValue?.icon ?? '',
                displayName: newValue?.label ?? '',
                typeName: newValue?.value ?? ''
            };

            props.value = tagManagerSnippet.typeName;

            if (props.onChange !== undefined) {
                props.onChange(props.value);
            }
        }
    }

    /* eslint-disable @typescript-eslint/naming-convention */
    const customStyle: StylesConfig<OptionType> = {
        control: (styles, { isFocused }) => ({
            ...styles,
            backgroundColor: 'white',
            borderColor: isFocused ? 'black' : 'gray',
            '&:hover': {
                borderColor: 'black'
            },
            borderRadius: 20,
            boxShadow: 'gray',
            padding: 2,
            minHeight: 'fit-content',
        }),
        option: (styles, { isSelected }) => {
            return {
                ...styles,
                backgroundColor: isSelected ? '#bab4f0' : 'white',
                '&:hover': {
                    backgroundColor: isSelected ? '#a097f7' : 'lightgray'
                },
                color: isSelected ? 'purple' : 'black',
                cursor: 'pointer'
            };
        },
        input: (styles) => ({ ...styles }),
        container: (styles) => ({ ...styles, borderColor: 'gray' }),
        placeholder: (styles) => ({ ...styles }),
        singleValue: (styles) => ({ ...styles }),
        indicatorSeparator: () => ({}),
        dropdownIndicator: (styles, state) => ({
            ...styles,
            transform: state.selectProps.menuIsOpen ? 'rotate(180deg)' : 'rotate(0deg)'
        })
    };
    /* eslint-enable @typescript-eslint/naming-convention */

    return (
        <div className="container___zbhlz">
            <div className="label-wrapper___AcszK">
                <div>
                    <label className="label___WET63" aria-disabled="false">
                        <span className="required___yY_P2">*</span>
                        <span>Tag type</span>
                    </label>
                </div>
            </div>
            <Select<OptionType, false>
                defaultValue={options.find(option => option.value === props.value)}
                options={options}
                styles={customStyle}
                components={{ SingleValue: CustomSingleValue, Option: CustomOption }}
                isSearchable={false}
                onChange={Save}
                placeholder="Select a tag type"
                theme={(theme) => ({
                    ...theme,
                    borderRadius: 0,
                    borderColor: 'gray',
                    colors: {
                        ...theme.colors,
                        primary25: 'gray',
                        primary: 'gray',
                    },
                })} />
        </div>
    );
};