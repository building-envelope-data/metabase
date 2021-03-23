import { Modal, Typography, List } from "antd";

const recoveryCodesModal = (recoveryCodes: string[]) => {
  Modal.success({
    title: "New Recovery Codes",
    content: (
      <div>
        <Typography.Paragraph strong>
          Put these codes in a safe place.
        </Typography.Paragraph>
        <Typography.Paragraph>
          If you lose your device and don't have the recovery codes you will
          lose access to your account.
        </Typography.Paragraph>
        <List>
          {recoveryCodes.map((code) => (
            <List.Item>{code}</List.Item>
          ))}
        </List>
      </div>
    ),
  });
};

export default recoveryCodesModal;
