import * as React from 'react';
import { Log, FormDisplayMode } from '@microsoft/sp-core-library';
import { FormCustomizerContext } from '@microsoft/sp-listview-extensibility';

import styles from './PaIntegration.module.scss';

export interface IPaIntegrationProps {
  context: FormCustomizerContext;
  displayMode: FormDisplayMode;
  onSave: () => void;
  onClose: () => void;
}

const LOG_SOURCE: string = 'PaIntegration';

export default class PaIntegration extends React.Component<IPaIntegrationProps, {}> {
  public componentDidMount(): void {
    Log.info(LOG_SOURCE, 'React Element: PaIntegration mounted');
  }

  public componentWillUnmount(): void {
    Log.info(LOG_SOURCE, 'React Element: PaIntegration unmounted');
  }

  public render(): React.ReactElement<{}> {
    return <div className={styles.paIntegration} />;
  }
}
