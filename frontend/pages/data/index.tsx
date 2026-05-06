import Layout from "../../components/Layout";
import { Flex } from "antd";
import paths from "../../paths";
import Link from "next/link";

const navItems = [
  {
    path: paths.allCalorimetricData,
    label: "Calorimetric Data",
  },
  {
    path: paths.allGeometricData,
    label: "Geometric Data",
  },
  {
    path: paths.allHygrothermalData,
    label: "Hygrothermal Data",
  },
  {
    path: paths.allLifeCycleData,
    label: "Life-Cycle Data",
  },
  {
    path: paths.allOpticalData,
    label: "Optical Data",
  },
  {
    path: paths.allPhotovoltaicData,
    label: "Photovoltaic Data",
  },
];

function Page() {
  return (
    <Layout>
      <Flex justify="center" gap="medium">
        {navItems.map((item) => (
          <Link key={item.path} href={item.path}>
            {item.label}
          </Link>
        ))}
      </Flex>
    </Layout>
  );
}

export default Page;
