import * as React from 'react';
import * as ReactDOM from 'react-dom';

import { FormDisplayMode, Log } from '@microsoft/sp-core-library';
import {
  BaseFormCustomizer
} from '@microsoft/sp-listview-extensibility';

import PaIntegration, { IPaIntegrationProps } from './components/PaIntegration';

import {
  SPHttpClient
} from '@microsoft/sp-http';

/**
 * If your form customizer uses the ClientSideComponentProperties JSON input,
 * it will be deserialized into the BaseExtension.properties object.
 * You can define an interface to describe it.
 */
export interface IPaIntegrationFormCustomizerProperties {
  // This is an example; replace with your own property
  sampleText?: string;
}

export interface IListItem {
  Id: number;
  Title: string;
  NewUrl: string;
  EditUrl: string;
  ViewUrl: string;
}

const LOG_SOURCE: string = 'PaIntegrationFormCustomizer';

export default class PaIntegrationFormCustomizer
  extends BaseFormCustomizer<IPaIntegrationFormCustomizerProperties> {

    public onInit(): Promise<void> {
      // Add your custom initialization to this method. The framework will wait
      // for the returned promise to resolve before rendering the form.
      Log.info(LOG_SOURCE, 'Activated ExtVariantTypesUkFormCustomizer with properties:');
      Log.info(LOG_SOURCE, JSON.stringify(this.properties, undefined, 2));

      this.context.spHttpClient
      .get(this.context.pageContext.web.absoluteUrl + `/_api/web/lists/getbytitle('PAURLSettings')/items?$filter=(ListID eq '${this.context.list.guid}')`, SPHttpClient.configurations.v1, {
        headers: {
          accept: 'application/json;odata.metadata=none'
        }
      })
      .then(res => {
        if (res.ok) {
          return res.json();
        }
        else {
          alert(res.statusText);
          return Promise.reject(res.statusText);
        }
      }).then((jsonResponse: any) => {
        const configs = jsonResponse.value as IListItem[];
        if(configs.length === 1)
        {
          const config = configs[0];
          if (this.displayMode === FormDisplayMode.New) {
            if(config.NewUrl)
            {
                window.open(config.NewUrl, '_blank');
            }
            this.formClosed();
          }
          if (this.displayMode === FormDisplayMode.Edit) {
            if(config.EditUrl)
            {
                window.open(`${config.EditUrl}${this.context.itemId}`, '_blank');
            }
            this.formClosed();
          }
          if (this.displayMode === FormDisplayMode.Display) {
            if(config.ViewUrl)
            {
                window.open(`${config.ViewUrl}${this.context.itemId}`, '_blank');
            }
            this.formClosed();
          }
        }
      });
     
      return Promise.resolve();
    }

  public render(): void {
    // Use this method to perform your custom rendering.

    const paIntegration: React.ReactElement<{}> =
      React.createElement(PaIntegration, {
        context: this.context,
        displayMode: this.displayMode,
        onSave: this._onSave,
        onClose: this._onClose
       } as IPaIntegrationProps);

    ReactDOM.render(paIntegration, this.domElement);
  }

  public onDispose(): void {
    // This method should be used to free any resources that were allocated during rendering.
    ReactDOM.unmountComponentAtNode(this.domElement);
    super.onDispose();
  }

  private _onSave = (): void => {

    // You MUST call this.formSaved() after you save the form.
    this.formSaved();
  }

  private _onClose =  (): void => {
    // You MUST call this.formClosed() after you close the form.
    this.formClosed();
  }
}
