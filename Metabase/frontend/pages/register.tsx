import { useRouter } from 'next/router'
import { initializeApollo } from '../lib/apollo'
import { useRegisterUserMutation } from '../lib/currentUser.graphql'
import { Form, Input, Button } from "antd"
import Layout from '../components/Layout'

const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
};
const tailLayout = {
    wrapperCol: { offset: 8, span: 16 },
};

function Register() {
    const router = useRouter()
    const apolloClient = initializeApollo()
    const [registerUserMutation] = useRegisterUserMutation()

    const onFinish = ({ email, password, passwordConfirmation }: any) => { // TODO `rememberMe`
        const register = async () => {
            try {
                // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
                await apolloClient.resetStore()
                const { data, errors } = await registerUserMutation({
                    variables: {
                        email: email,
                        password: password,
                        passwordConfirmation: passwordConfirmation,
                    },
                })
                if (errors) {
                    console.log('Failed:', errors)
                }
                if (data?.registerUser?.errors) {
                    console.log('Failed:', data?.registerUser?.errors)
                }
                if (data?.registerUser?.user) {
                    await router.push('/')
                }
            } catch (error) {
                console.log('Failed:', error)
            }
        }
        register()
    };

    const onFinishFailed = (errorInfo: any) => {
        console.log('Failed:', errorInfo)
    };

    return (
        <Layout>
            <Form
                {...layout}
                name="basic"
                onFinish={onFinish}
                onFinishFailed={onFinishFailed}
            >
                <Form.Item
                    label="Email"
                    name="email"
                    rules={[
                        {
                            type: 'email',
                            message: 'Invalid email!',
                        },
                        {
                            required: true,
                            message: 'Please input your email!'
                        }
                    ]}
                >
                    <Input />
                </Form.Item>

                <Form.Item
                    label="Password"
                    name="password"
                    rules={[{ required: true, message: 'Please input your password!' }]}
                >
                    <Input.Password />
                </Form.Item>

                <Form.Item
                    label="Confirm Password"
                    name="passwordConfirmation"
                    dependencies={['password']}
                    rules={[
                        {
                            required: true,
                            message: 'Please input your password!'
                        },
                        ({ getFieldValue }) => ({
                            validator(_, value) {
                                if (!value || getFieldValue('password') === value) {
                                    return Promise.resolve();
                                }
                                return Promise.reject('Password and confirmation do not match!');
                            },
                        }),
                    ]}
                >
                    <Input.Password />
                </Form.Item>

                <Form.Item {...tailLayout}>
                    <Button type="primary" htmlType="submit">
                        Submit
                </Button>
                </Form.Item>
            </Form>
        </Layout>
    );
}

export default Register