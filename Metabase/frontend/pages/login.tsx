import { useRouter } from 'next/router'
import { initializeApollo } from '../lib/apollo'
import { useLoginUserMutation } from '../lib/currentUser.graphql'
import { Form, Input, Button, Checkbox } from "antd"
import Layout from '../components/Layout'

const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
};
const tailLayout = {
    wrapperCol: { offset: 8, span: 16 },
};

function Login() {
    const router = useRouter()
    const apolloClient = initializeApollo()
    const [loginUserMutation] = useLoginUserMutation()

    const onFinish = ({ email, password }: any) => { // TODO `rememberMe`
        const login = async () => {
            try {
                // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
                await apolloClient.resetStore()
                const { data, errors } = await loginUserMutation({
                    variables: {
                        email: email,
                        password: password,
                    },
                })
                if (errors) {
                    console.log('Failed:', errors)
                }
                if (data?.loginUser?.errors) {
                    console.log('Failed:', data?.loginUser?.errors)
                }
                if (data?.loginUser?.user) {
                    await router.push('/')
                }
            } catch (error) {
                console.log('Failed:', error)
            }
        }
        login()
    };

    const onFinishFailed = (errorInfo: any) => {
        console.log('Failed:', errorInfo)
    };

    return (
        <Layout>
            <Form
                {...layout}
                name="basic"
                initialValues={{ remember: true }}
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
                    {...tailLayout}
                    name="rememberMe"
                    valuePropName="checked"
                >
                    <Checkbox>Remember me</Checkbox>
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

export default Login