export const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};

export const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export const getFieldValue = (values: any, path: (string | number)[]) => {
  return path.reduce((obj, key) => obj?.[key], values);
};
