import { Typography } from "antd";
import Icon, { BarcodeOutlined, GlobalOutlined } from "@ant-design/icons";
import { CustomIconComponentProps } from "@ant-design/icons/lib/components/Icon";
import { Iconize } from "./Iconize";
import { addPrefix, removePrefix } from "../lib/string";
import CopyButton from "./CopyButton";

const ArXivSvg = () => (
  <svg width="1em" height="1em" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12.4 12.8L18.4 2H22L14.2 13.8L22 22H18.4L12.4 14.8L6.4 22H2.8L10.6 11.2L2.8 2H6.4L12.4 12.8Z" />
  </svg>
);

const DoiSvg = () => (
  <svg width="1em" height="1em" fill="currentColor" viewBox="0 0 24 24">
    <path d="M12 0C5.373 0 0 5.373 0 12s5.373 12 12 12 12-5.373 12-12S18.627 0 12 0zm0 2c5.523 0 10 4.477 10 10s-4.477 10-10 10S2 17.523 2 12 6.477 2 12 2zm-1.8 14.2h1.6V7.8h-1.6v8.4zm4.8-4.2c0 2.319-1.881 4.2-4.2 4.2h-.4v-1.6h.4c1.436 0 2.6-1.164 2.6-2.6s-1.164-2.6-2.6-2.6h-.4V7.8h.4c2.319 0 4.2 1.881 4.2 4.2z" />
  </svg>
);

const ArXivIcon = (props: Partial<CustomIconComponentProps>) => (
  <Icon component={ArXivSvg} {...props} />
);
const DoiIcon = (props: Partial<CustomIconComponentProps>) => (
  <Icon component={DoiSvg} {...props} />
);

const IDENTIFIER_CONFIG = {
  arXiv: {
    label: null,
    prefix: "arXiv:",
    icon: <ArXivIcon style={{ color: "#b31b1b" }} />,
    url: ({ valueWithoutPrefix }: { valueWithoutPrefix: string }) =>
      `https://arxiv.org/abs/${valueWithoutPrefix}`,
  },
  doi: {
    label: null,
    prefix: "doi:",
    icon: <DoiIcon style={{ color: "#ff6600" }} />,
    url: ({ valueWithoutPrefix }: { valueWithoutPrefix: string }) =>
      `https://doi.org/${valueWithoutPrefix}`,
  },
  urn: {
    label: null,
    prefix: "urn:",
    icon: <BarcodeOutlined style={{ color: "#1890ff" }} />,
    url: ({
      valueWithPrefix,
      valueWithoutPrefix,
    }: {
      valueWithPrefix: string;
      valueWithoutPrefix: string;
    }) => {
      if (valueWithPrefix.startsWith("urn:isbn:")) {
        return `https://isbnsearch.org/isbn/${removePrefix(valueWithoutPrefix, "isbn:")}`;
      }
      if (valueWithPrefix.startsWith("urn:issn:")) {
        return `https://urn.issn.org/${valueWithPrefix}`;
      }
      if (valueWithPrefix.startsWith("urn:nbn:")) {
        return `https://nbn-resolving.org/${valueWithPrefix}`;
      }
      return `https://www.ecosia.org/search?q=${valueWithPrefix}`;
    },
  },
  webAddress: {
    label: "Web",
    prefix: "https://",
    icon: <GlobalOutlined />,
    url: ({ value }: { value: string }) => value,
  },
};

export default function IdentifierItem({
  type,
  value,
}: {
  type: keyof typeof IDENTIFIER_CONFIG;
  value: string;
}) {
  const config = IDENTIFIER_CONFIG[type];
  const valueWithoutPrefix = removePrefix(value, config.prefix);
  const valueWithPrefix = addPrefix(value, config.prefix);
  const href = config.url({ value, valueWithPrefix, valueWithoutPrefix });

  return (
    <span>
      <Typography.Link href={href} target="_blank">
        <Iconize icon={config.icon}>
          {config.label ? config.label : valueWithPrefix}
        </Iconize>
      </Typography.Link>
      <CopyButton onlyIcon getText={() => value} />
    </span>
  );
}
