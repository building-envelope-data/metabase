import { useRouter } from 'next/router'
import { initializeApollo } from '../../lib/apollo'
import { useChangeUserEmailMutation } from '../../queries/currentUser.graphql'
import { Form, Input, Button } from "antd"
import Layout from '../../components/Layout'

const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
};
const tailLayout = {
    wrapperCol: { offset: 8, span: 16 },
};

function ChangeUserEmail() {
    const router = useRouter()
    const apolloClient = initializeApollo()
    const [changeUserEmailMutation] = useChangeUserEmailMutation()

    const onFinish = ({ newEmail }: any) => { // TODO `rememberMe`
        const changeUserEmail = async () => {
            try {
                // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
                await apolloClient.resetStore()
                const { data, errors } = await changeUserEmailMutation({
                    variables: {
                        newEmail: newEmail,
                    },
                })
                if (errors) {
                    console.log('Failed:', errors)
                }
                if (data?.changeUserEmail?.errors) {
                    console.log('Failed:', data?.changeUserEmail?.errors)
                }
                if (data?.changeUserEmail?.user) {
                    await router.push('/')
                }
            } catch (error) {
                console.log('Failed:', error)
            }
        }
        changeUserEmail()
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
                    label="New Email"
                    name="newEmail"
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

                <Form.Item {...tailLayout}>
                    <Button type="primary" htmlType="submit">
                        Submit
                </Button>
                </Form.Item>
            </Form>
        </Layout>
    );
}

export default ChangeUserEmail